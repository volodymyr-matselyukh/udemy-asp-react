import { makeAutoObservable, runInAction } from "mobx";
import agent from "../api/agent";
import { User, UserFormValues } from "../models/user";
import { store } from "./store";
import { browserHistory } from "../../index";

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
            browserHistory.push('/activities');
            store.modalStore.closeModal();
            
        } catch (error) {
            throw error;
        }
    }

    logout = () => {
        store.commonStore.setToken(null);
        window.localStorage.removeItem('jwt');
        this.user = null;
        browserHistory.push('/');
    }

    getUser = async () => {
        try {
            const user = await agent.Account.current();
            runInAction(() => this.user = user);
        } catch (error) {
            console.log(error);
        }
    }

    register = async (creds: UserFormValues) => {
        try{
            const user = await agent.Account.register(creds);
            runInAction(() => {
                this.user = user;
                store.commonStore.setToken(user.token);
            });
            browserHistory.push('/activities');
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
}