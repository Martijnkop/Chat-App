import React, { useState } from 'react';
import ReactDOM from 'react-dom';
import './index.css';
import App from './App';
import { Provider } from "react-redux";
import store from "./Core/app.reducer";
import {
  BrowserRouter as Router,
  Switch,
  Route
} from "react-router-dom";

import Login from './Main/Login/Login';
import Register from './Main/Register/Register';
import Logout from './Main/Logout/Logout';

import Connect from './Main/Connect';

import Main from './Main/MainApp/Main';

let connection;
let setConnection = (conn) => {
  connection = conn
}

ReactDOM.render(
  <Provider store={store}>
    <React.StrictMode>
      <div className={localStorage.getItem('darkmode') === "true" ? "app dark" : "app light"} >
        <Connect setConnection = {setConnection} {...connection}>
        <Router>
          <Switch>
            <Route exact path="/" >
              <App {...connection} />
            </Route>
            <Route exact path="/login" >
              <Login {...connection} />
            </Route>
            <Route exact path="/logout" >
              <Logout {...connection} />
            </Route>
            <Route exact path="/register" >
              <Register {...connection} />
            </Route>
            <Route path="/app" >
              <Main connection = {connection} />
            </Route>
          </Switch>
        </Router>
        </Connect>
      </div>
    </React.StrictMode>
  </Provider>,
  document.getElementById('root')
);