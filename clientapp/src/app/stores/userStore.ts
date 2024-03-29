import { makeAutoObservable, runInAction } from "mobx";
import agent from "../api/agent";
import { User, UserFormValues } from "../models/user";
import { store } from "./store";
import { router } from "../layout/App";

export default class UserStore{
    user: User | null = null;

    constructor() {
        makeAutoObservable(this);
    }

    get isLoggedIn(){
        return !!this.user;
    }

    login = async (creds: UserFormValues) => {
        try{
            const user = await agent.Account.login(creds);
            runInAction(() => {
                this.user = user;
                store.commonStore.setToken(user.token);
            });

			store.modalStore.closeModal();
			return router.navigate('/activities');
        } catch (error) {
            throw error;
        }
    }

    logout = () => {
        this.logoutUser();
		router.navigate('/');
    }

	logoutUser = () => {
		store.commonStore.setToken(null);
        window.localStorage.removeItem('jwt');
		store.activityStore.activityRegistry.clear();
        this.user = null;
	}

    getUser = async () => {
		const user = await agent.Account.current();
		runInAction(() => this.user = user);
    }

    register = async (creds: UserFormValues) => {
        try{
            const user = await agent.Account.register(creds);
            runInAction(() => {
                this.user = user;
                store.commonStore.setToken(user.token);
            });
			router.navigate('/activities');
            store.modalStore.closeModal();
        } catch (error) {
            throw error;
        }
    }

	setImage = (image: string) => {
		if(this.user)
		{
			this.user.image = image;
		}
	}

	setDisplayName = (displayName: string) => {
		if(this.user)
		{
			this.user.displayName = displayName;
		}
	}
}