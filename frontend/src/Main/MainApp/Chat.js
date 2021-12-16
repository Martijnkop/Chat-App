import Chatbox from '../Components/Chatbox/Chatbox';
import Message from '../Components/Message/Message';

import './Chat.css';
import { connect } from 'react-redux';

import { useEffect, useState } from 'react';

import { getGlobalConnection } from '../../Core/Global/global.selectors';

function Chat(props) {
    console.log(props)
    let username = sessionStorage.getItem('username')

    async function submit(value) {
        console.log("output: " + value)
        try {
            await props.connection.send("SendMessage", props.token, props.activeChat, value)
        } catch (e) {
            console.log(e)
        }
    }

    const [sessionIdSaved, setSessionIdSaved] = useState(false)
    useEffect(() => {
        if (props.connection !== undefined && !sessionIdSaved) {
            props.connection.send('SaveSessionID', username)
            setSessionIdSaved(true)
        }
    }, [sessionIdSaved, setSessionIdSaved])

    return (
        <div className="Chat">
            <Chatbox onSubmit={submit}/>
            
            {props.messages.map(m => {
                return (<Message {...m} me = {username} className="message" />)
            })}
        </div>
    );
}

const mapStateToProps = function(state) {
    return {
        connection: getGlobalConnection(state)
    }
}

export default connect(mapStateToProps)(Chat);