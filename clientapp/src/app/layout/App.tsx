import React, { useEffect } from "react";
import "./App.css";
import { Container } from "semantic-ui-react";
import NavBar from "./NavBar";
import { observer } from "mobx-react-lite";

import HomePage from "../../features/home/HomePage";
import { Route, Routes } from "react-router-dom";
import ActivityDashboard from "../../features/activities/dashboard/ActivityDashboard";
import ActivityForm from "../../features/activities/form/ActivityForm";
import ActivityDetails from "../../features/activities/details/ActivityDetails";
import TestErrors from "../../features/errors/TestErrors";
import { ToastContainer } from "react-toastify";
import NotFound from "../../features/errors/NotFound";
import ServerError from "../../features/errors/ServerError";
import LoginForm from "../../features/users/LoginForm";
import { useStore } from "../stores/store";
import LoadingComponent from "./LoadingComponents";
import ModalContainer from "../common/modals/ModalContainer";
import ProfilePage from "../../features/profiles/ProfilePage";

function App() {
	const { commonStore, userStore } = useStore();

	useEffect(() => {
		if (commonStore.token) {
			userStore
				.getUser()
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
			<Routes>
				<Route path="/" element={<HomePage />} />
				<Route
					path={"*"}
					element={
						<>
							<NavBar />

							<Container style={{ marginTop: "7em" }}>
								<Routes>
									<Route
										path="/activities"
										element={<ActivityDashboard />}
									/>
									<Route
										path="/activities/:id"
										element={<ActivityDetails />}
									/>
									<Route
										path="/createActivity"
										element={<ActivityForm />}
									/>
									<Route
										path="/manage/:id"
										element={<ActivityForm />}
									/>
									<Route
										path="/profiles/:username"
										element={<ProfilePage />}
									/>
									<Route
										path="/errors"
										element={<TestErrors />}
									/>
									<Route
										path="/server-error"
										element={<ServerError />}
									/>
									<Route
										path="/login"
										element={<LoginForm />}
									/>
									<Route path={"*"} element={<NotFound />} />
								</Routes>
							</Container>
						</>
					}
				/>
			</Routes>
		</>
	);
}

export default observer(App);
