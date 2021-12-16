import React from 'react';
import { CircularProgress } from '@material-ui/core';
import "./Loading.css";

function Loading(props) {
    return (<div className={"LoadingBackground " + props.className} >
        <CircularProgress className="LoadingCircle" size="80px" />
        <p className="LoadingText">{props.loadingMessage ? props.loadingMessage : "Loading..."}</p>
    </div>)
}

export default Loading;