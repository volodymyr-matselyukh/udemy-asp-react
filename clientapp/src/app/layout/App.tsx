import { useEffect } from "react";
import "./App.css";
import { Container } from "semantic-ui-react";
import NavBar from "./NavBar";
import { observer } from "mobx-react-lite";

import HomePage from "../../features/home/HomePage";
import {
	createBrowserRouter,
	createRoutesFromElements,
	Outlet,
	Route,
	RouterProvider,
	ScrollRestoration,
} from "react-router-dom";
import ActivityDashboard from "../../features/activities/dashboard/ActivityDashboard";
import ActivityForm from "../../features/activities/form/ActivityForm";
import ActivityDetails from "../../features/activities/details/ActivityDetails";
import TestErrors from "../../features/errors/TestErrors";
import { ToastContainer } from "react-toastify";
import NotFound from "../../features/errors/NotFound";
import ServerError from "../../features/errors/ServerError";
import { useStore } from "../stores/store";
import LoadingComponent from "./LoadingComponents";
import ModalContainer from "../common/modals/ModalContainer";
import ProfilePage from "../../features/profiles/ProfilePage";
import RequireAuth from "../router/RequireAuth";

export const router = createBrowserRouter(
	createRoutesFromElements(
		<>
			<Route path="/" element={<HomePage />}></Route>
			<Route
				path="/"
				element={
					<>
						<ScrollRestoration />
						<NavBar />

						<Container style={{ marginTop: "7em" }}>
							<Outlet />
						</Container>
					</>
				}
			>
				<Route element={<RequireAuth />}>
					<Route path="/activities" element={<ActivityDashboard />} />
					<Route
						path="/activities/:id"
						element={<ActivityDetails />}
					/>
					<Route path="/createActivity" element={<ActivityForm />} />
					<Route path="/manage/:id" element={<ActivityForm />} />
					<Route
						path="/profiles/:username"
						element={<ProfilePage />}
					/>
					<Route path="/errors" element={<TestErrors />} />
				</Route>

				<Route path="/server-error" element={<ServerError />} />
				<Route path="*" element={<NotFound />} />
			</Route>
		</>
	)
);

function App() {
	const { commonStore, userStore } = useStore();

	useEffect(() => {
		if (commonStore.token) {
			userStore
				.getUser()
				.catch((error) => {
					if (error && (error as any).response?.status === 401) {
						userStore.logoutUser();
					}
				})
				.finally(() => {
					commonStore.setAppLoaded();
				});
		} else {
			commonStore.setAppLoaded();
		}
	}, [commonStore, userStore]);

	if (!commonStore.appLoaded)
		return <LoadingComponent content="Loading app..." />;

	return (
		<>
			<ToastContainer position="bottom-right" hideProgressBar />
			<ModalContainer />
			<RouterProvider router={router} />
		</>
	);
}

export default observer(App);
