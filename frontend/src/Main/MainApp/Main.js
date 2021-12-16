import Chat from './Chat';
import Sidebar from './Sidebar';

import { useCallback, useEffect, useState } from 'react';


import { connect } from "react-redux";
import { getGlobalConnection } from "../../Core/Global/global.selectors";

import "./Main.css";

import https from 'https';


function Main(props) {
    const [friends, setFriends] = useState([]);
    const [newToken, setNewToken] = useState("");
    const [account, setAccount] = useState(localStorage.getItem('account'));
    const [active, setActive] = useState('');
    const [messages, setMessages] = useState([]);

    const setNewActive = useCallback((newActive) => {
        setActive(newActive)

        const getNewActiveMessagesOptions = {
            hostname: 'localhost',
            port: 5001,
            path: `/getactivemessages/${newActive}`,
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                'Content-Length': newToken.length
            }
        }

        const getNewActiveMessages = https.request(getNewActiveMessagesOptions, newActiveMessagesResult => {
            newActiveMessagesResult.on('data', newActiveMessageData => {
                var m = (JSON.parse(new TextDecoder().decode(newActiveMessageData)));
                setMessages(m)
                
            })
        })

        getNewActiveMessages.setHeader('token', newToken);

        getNewActiveMessages.on('error', error => console.error(error))

        getNewActiveMessages.end()

    }, [active])

    useEffect(() => {
        const oldToken = localStorage.getItem('account')
        const optionsNewTokenRequest = {
            hostname: 'localhost',
            port: 5003,
            path: '/getaudiencetoken/main.chatapp.com',
            method: 'GET'
        }

        const newTokenRequest = https.request(optionsNewTokenRequest, newTokenResult => {
            newTokenResult.on('data', newTokenData => {
                setNewToken(new TextDecoder().decode(newTokenData));

                console.log(newToken)
                

                const optionsFriends = {
                    hostname: 'localhost',
                    port: 5001,
                    path: '/friends',
                    method: 'GET',
                    headers: {
                        'Content-Type': 'application/json',
                        'Content-Length': newToken.length
                    }
                }
        
                const friendsRequest = https.request(optionsFriends, friendsResult => {
                    friendsResult.on('data', friendData => {
                        setFriends(JSON.parse(new TextDecoder().decode(friendData)));
                    });
                });
        
                friendsRequest.setHeader('token', newToken);
        
                friendsRequest.on('error', error => {
                    console.error(error);
                });
        
                friendsRequest.write(newToken);
                friendsRequest.end();
            });
        });

        newTokenRequest.setHeader('token', oldToken);

        newTokenRequest.on('error', error => {
            console.error(error)
        })

        newTokenRequest.write(oldToken);
        newTokenRequest.end();

        
    }, [newToken, account])

    if (props.connection === undefined) return (<></>)


    props.connection.on('GetMessage', () => {
        console.log('test')
        //if (user === active) {
            let temp = [...messages]
            //temp.push(message)
                
            setMessages(temp)
        //}
    })

    return (
    <div className = "Main">
        <Sidebar friends = {friends} active = {active} setNewActive = {setNewActive} />
        <Chat activeChat = {active} token = {newToken} messages = {messages} connection = {props.connection}/>
    </div>
    );
}

const mapStateToProps = (state) => {
    return { connection: getGlobalConnection(state) }
}

export default connect(mapStateToProps)(Main);