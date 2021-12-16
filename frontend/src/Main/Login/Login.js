import React, { useState } from 'react';
import https from 'https';

import './Login.css';
import TextInput from '../Components/TextInput/TextInput'
import Button from '../Components/Button/Button';
import { Redirect } from 'react-router';

export default function Login() {
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [emailWarning, setEmailWarning] = useState(false)
    const [passwordWarning, setPasswordWarning] = useState(false)
    const [goToRegister, setRegister] = useState(false)

    const [redirectToMain, setRedirectToMain] = useState(false)

    const login = () => {
        setEmailWarning(email === "")
        setPasswordWarning(password === "")

        if (!emailWarning && !passwordWarning) {
            var account = {}
            account.Email = email;
            account.Password = password

            const data = JSON.stringify(account)

            const options = {
                hostname: 'localhost',
                port: 5003,
                path: '/login',
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Content-Length': data.length
                }
            }

            const req = https.request(options, res => {
                console.log(res)
                res.on('data', d => {
                    var returndata = new TextDecoder().decode(d);
                    var parsedData = JSON.parse(returndata)


                    var content = JSON.parse(`${Buffer.from(parsedData.generatedToken.split(".")[1], 'base64')}`)
                    sessionStorage.setItem('username', content.name)
                    

                    localStorage.setItem('account', parsedData.generatedToken);
                    localStorage.setItem('refreshToken', parsedData.refreshToken.join(""));

                    setRedirectToMain(true)
                })
            })

            req.on('error', error => {
                console.error(error)
            })

            req.write(data)
            req.end()
        }
    }

    const register = () => {
        setRegister(true)
    }

    if (goToRegister) return (<Redirect to={{ pathname: "/register" }} />)
    if (redirectToMain) return (<Redirect to={{ pathname: "/" }} />)

    return (
        <form className="card">
            <p className="logintext">Login:</p>
            <TextInput
                placeholder="Email"
                handleChange={setEmail}
                mainStyle={{ width: '75%' }}
                inputStyle={{ backgroundColor: '#F37F68' }}
                warning={emailWarning}
                autocomplete="username" />
            <TextInput
                placeholder="Password"
                handleChange={setPassword}
                mainStyle={{ width: '75%', marginBottom: "30px" }}
                inputStyle={{ backgroundColor: '#F37F68' }}
                hidden={true}
                warning={passwordWarning}
                autocomplete="current-password" />
            <div className="buttons">
                <Button text="login" click={login} />
                <Button text="register" click={register} />
            </div>
        </form>)
}