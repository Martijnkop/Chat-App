import React from 'react';

import { library } from '@fortawesome/fontawesome-svg-core';
import * as Icons from '@fortawesome/free-solid-svg-icons'

import { connect } from "react-redux";
import { setConnection } from "../Core/Global/global.actions";
import { HubConnectionBuilder } from '@microsoft/signalr';
import { getGlobalConnection } from "../Core/Global/global.selectors";



class Connect extends React.Component {
    constructor(props) {
        super(props)
    }

    componentWillMount() {
        const connection = new HubConnectionBuilder()
            .withUrl('https://localhost:5001/hubs/messages')
            .withAutomaticReconnect()
            .build();

        connection.start()
            .then(result => {
                this.props.dispatch(setConnection(connection))
                console.log(connection)
            })
    }

    render() {
        return ({...this.props.children})
    }
}

const iconList = Object.keys(Icons)
  .filter((key) => key !== 'fas' && key !== 'prefix' && key !== 'font-awesome-logo-full')
  .map((icon) => Icons[icon]);

library.add(...iconList);


const mapStateToProps = (state) => {
    return { connection: getGlobalConnection(state) }
}

export default connect(mapStateToProps)(Connect);