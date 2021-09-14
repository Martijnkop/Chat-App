import './App.css';


import React from 'react';

import { library } from '@fortawesome/fontawesome-svg-core';
import * as Icons from '@fortawesome/free-solid-svg-icons';

import { connect } from "react-redux";
import { HubConnectionBuilder } from '@microsoft/signalr';
import { getGlobalConnection } from "./Core/Global/global.selectors";
import { setConnection } from "./Core/Global/global.actions";

import Loading from "./Main/Loading"

import SetLocalStorage from './Core/LocalStrorage/SetLocalStorage';
import GetLocalStorage from './Core/LocalStrorage/GetLocalStorage';

var storage = {};

class App extends React.Component {
  loaded = false;

  constructor(props) {
    super(props)

    if (!localStorage.getItem('darkmode')) {
      storage = SetLocalStorage();
    } else {
      storage = GetLocalStorage();
    }

    this.state = {
      loadingMessage: "Connecting to Server..."
    }
  }

  componentDidMount() {
    const connection = new HubConnectionBuilder()
      .withUrl('https://localhost:5001/hubs/messages')
      .withAutomaticReconnect()
      .build();

    connection.start()
      .then(result => {
        this.props.dispatch(setConnection(connection))

        this.setState({
          loadingMessage: "Getting Account Data..."
        })
      })
      .catch(e => console.info(e))

    connection.on("SendAccountData", data => {
      console.log(data)
    })
  }

  render() {
    console.log(storage)
    if (!this.loaded) {
      return (<Loading className={storage.darkmode === "true" ? "dark" : "light"} loadingMessage={this.state.loadingMessage} />)
    }
    return (
      <div className={(storage.darkmode === "true") ? "app dark" : "app light"}>
      </div>
    );
  }
}

const iconList = Object.keys(Icons)
  .filter((key) => key !== 'fas' && key !== 'prefix' && key !== 'font-awesome-logo-full')
  .map((icon) => Icons[icon]);

library.add(...iconList);

const mapStateToProps = (state) => {
  return { connection: getGlobalConnection(state) }
}

export default connect(mapStateToProps)(App);