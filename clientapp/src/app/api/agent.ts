import axios, { AxiosError, AxiosResponse } from "axios";
import { toast } from "react-toastify";
import { browserHistory } from "../..";
import { Activity, ActivityFormValues } from "../models/activity";
import { PaginatedResult } from "../models/pagination";
import { Photo, Profile } from "../models/profile";
import { User, UserFormValues } from "../models/user";
import { store } from "../stores/store";

const sleep = (delay: number) => {
	return new Promise((resolve) => {
		setTimeout(resolve, delay);
	});
}

axios.defaults.baseURL = "http://localhost:5000/api";

axios.interceptors.request.use(config => {
	const token = store.commonStore.token;
	if (token && config.headers) {
		config.headers.Authorization = `Bearer ${token}`;
	}

	return config;
})

axios.interceptors.response.use(async response => {
	await sleep(1000);
	const pagination = response.headers['pagination'];

	if(pagination){
		response.data = new PaginatedResult(response.data, JSON.parse(pagination));
		return response as AxiosResponse<PaginatedResult<any>>;
	}

	return response;
}, (error: AxiosError) => {
	let data, status, config;

	if(error.response)
	{
		data = error.response.data;
		status = error.response.status;
		config = error.response.config;
	}
	 

	switch (status) {
		case 400:
			if (typeof (data) === 'string') {
				toast.error(data);
			}

			if (config && config.method === 'get' && data.errors.hasOwnProperty('id')) {
				browserHistory.push('/not-found');
			}

			if (data.errors) {
				const modalStateErrors = [];
				for (const key in data.errors) {
					if (data.errors[key]) {
						modalStateErrors.push(data.errors[key]);
					}
				}

				throw modalStateErrors.flat();
			}
			break;
		case 401:
			if(config && config.url !== "/account")
			{
				toast.error('unauthorized', { theme: "colored" });
			}
			
			break;
		case 403:
			if(config && config.url !== "/account")
			{
				toast.error('unauthorized', { theme: "colored" });
			}
			
			break;
		case 404:
			browserHistory.push('/not-found');
			break;
		case 500:
			store.commonStore.setServerError(data);
			browserHistory.push('/server-error');
			break;
	}

	return Promise.reject(error);
});

const responseBody = <T>(response: AxiosResponse<T>) => response.data;

const requests = {
	get: <T>(url: string) => axios.get<T>(url).then(responseBody),
	post: <T>(url: string, body: {}) => axios.post<T>(url, body).then(responseBody),
	put: <T>(url: string, body: {}) => axios.put<T>(url, body).then(responseBody),
	del: <T>(url: string) => axios.delete<T>(url).then(responseBody),
}

const Activities = {
	list: (params: URLSearchParams) => axios.get<PaginatedResult<Activity[]>>('/activities', {params})
										.then(responseBody),
	details: (id: string) => requests.get<Activity>(`/activities/${id}`),
	create: (activity: ActivityFormValues) => requests.post<Activity>("/activities", activity),
	update: (activity: ActivityFormValues) => requests.put<Activity>(`/activities/${activity.id}`, activity),
	delete: (id: string) => requests.del<string>(`/activities/${id}`),
	attend: (id: string) => requests.post<void>(`/activities/${id}/attend`, {})
}

const Account = {
	current: () => requests.get<User>('/account'),
	login: (user: UserFormValues) => requests.post<User>('/account/login', user),
	register: (user: UserFormValues) => requests.post<User>('/account/register', user)
}

const Profiles = {
	get: (username: string) => requests.get<Profile>(`/profiles/${username}`),
	uploadPhoto: (file: Blob) => {
		let formData = new FormData();
		formData.append('File', file);
		return axios.post<Photo>('photos', formData, {
			headers: { 'Content-Type': 'multipart/form-data' }
		});
	},
	setMainPhoto: (id: string) => requests.post(`/photos/${id}/setMain`, {}),
	deletePhoto: (id: string) => requests.del(`/photos/${id}`),
	updateProfile: (profile: Partial<Profile>) => requests.put<void>("/profiles", profile),
	updateFollowing: (username: string) => requests.post(`/follow/${username}`, {}),
	listFollowings: (username: string, predicate: string) => requests.get<Profile[]>(`/follow/${username}?predicate=${predicate}`)
}

const agent = {
	Activities,
	Account,
	Profiles
}

export default agent;