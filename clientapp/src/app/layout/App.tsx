import React from "react";
import "./App.css";
import { Container } from "semantic-ui-react";
import NavBar from "./NavBar";
import { observer } from "mobx-react-lite";

import HomePage from "../../features/home/HomePage";
import { Route, Routes, useLocation } from "react-router-dom";
import ActivityDashboard from "../../features/activities/dashboard/ActivityDashboard";
import ActivityForm from "../../features/activities/form/ActivityForm";
import ActivityDetails from "../../features/activities/details/ActivityDetails";
import TestErrors from "../../features/errors/TestErrors";
import { ToastContainer } from "react-toastify";
import NotFound from "../../features/errors/NotFound";
import ServerError from "../../features/errors/ServerError";

function App() {
  return (
    <>
      <ToastContainer position='bottom-right' hideProgressBar />
      <Routes>
        <Route path="/" element={<HomePage />} />
        <Route
          path={"*"}
          element={
            <>
              <NavBar />

              <Container style={{ marginTop: "7em" }}>
                <Routes>
                  <Route path="/activities" element={<ActivityDashboard />} />
                  <Route path="/activities/:id" element={<ActivityDetails />} />
                  <Route path="/createActivity" element={<ActivityForm />} />
                  <Route path="/manage/:id" element={<ActivityForm />} />
                  <Route path="/errors" element={<TestErrors />} />
                  <Route path="/server-error" element={<ServerError />} />
                  <Route path={"*"} element={ <NotFound /> } />
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
