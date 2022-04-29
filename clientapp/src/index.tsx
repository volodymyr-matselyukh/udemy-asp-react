import React from "react";
import ReactDOM from "react-dom";
import 'react-calendar/dist/Calendar.css';
import "semantic-ui-css/semantic.min.css";
import "react-toastify/dist/ReactToastify.min.css";
import App from "./app/layout/App";
import { store, StoreContext } from "./app/stores/store";
import reportWebVitals from "./reportWebVitals";
import { createBrowserHistory } from "history";
import { HistoryRouter } from "./app/router/HistoryRouter";

export const browserHistory = createBrowserHistory();

ReactDOM.render(
  <StoreContext.Provider value={store}>
    <HistoryRouter history={browserHistory}>
      <App />
    </HistoryRouter>
  </StoreContext.Provider>,
  document.getElementById("root")
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
