import {
    Redirect
} from 'react-router';

import { useState } from 'react';

import https from 'https';

export default function Logout() {
    const [login, setLogin] = useState(false)
    let data = localStorage.getItem('refreshToken')
    if (data != '') {
        console.log(data)
        const options = {
            hostname: 'localhost',
            port: 5003,
            path: '/logout',
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                'Content-Length': data.length
            }
        }

        const req = https.request(options, res => {
            res.on('data', d => {
                console.log(new TextDecoder().decode(d))
                localStorage.setItem('account', "");
                localStorage.setItem("refreshToken", "");

                setLogin(true)
            })
        })

        req.on('error', error => {
            console.error(error)
        })

        req.setHeader('refreshToken', data)
        req.end()
    }

    if (login) {
        return <Redirect to="/login" />
    }
    return (<div />)
}