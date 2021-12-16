import { useState } from 'react';
import './Register.css';
import https from 'https';

import TextInput from '../Components/TextInput/TextInput';
import Button from '../Components/Button/Button';
import { Redirect } from 'react-router-dom';

function handleRegister(account) {
    const data = JSON.stringify(account)

    const options = {
        hostname: 'localhost',
        port: 5003,
        path: '/auth/register',
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Content-Length': data.length
        }
    }

    const req = https.request(options, res => {
        console.log(`result:`)
        res.on('data', d => {
            console.log(d.toString())


            return true;
        })
    })

    req.on('error', error => {
        console.error(error)
    })

    req.write(data)
    req.end()
}

export default function Register() {
    const [email, setEmail] = useState("")
    const [userName, setUserName] = useState("")
    const [firstName, setFirstName] = useState("")
    const [lastName, setLastName] = useState("")
    const [password, setPassword] = useState("")
    const [confirmPassword, setConfirmPassword] = useState("")


    const [emailWarning, setEmailWarning] = useState(false)
    const [userNameWarning, setUserNameWarning] = useState(false)
    const [firstNameWarning, setFirstNameWarning] = useState(false)
    const [lastNameWarning, setLastNameWarning] = useState(false)
    const [passwordWarning, setPasswordWarning] = useState(false)
    const [confirmPasswordWarning, setConfirmPasswordWarning] = useState(false)

    const [userNameWarningMessage, setUserNameWarningMessage] = useState()
    const [confirmWarningMessage, setConfirmWarningMessage] = useState()


    const register = () => {
        let valid = true
        if (email === "") {
            setEmailWarning(true)
            valid = false;
        } else setEmailWarning(false)

        if (userName.length <= 3) {
            setUserNameWarningMessage("Username is too short!")
            setUserNameWarning(true)
            valid = false
        } else {
            setUserNameWarningMessage()
            setUserNameWarning(false)
        }

        if (userName === "") {
            setUserNameWarning(true)
            setUserNameWarningMessage()
            valid = false
        } else setUserNameWarning(false)
        if (firstName === "") {
            setFirstNameWarning(true)
            valid = false
        } else setFirstNameWarning(false)
        if (lastName === "") {
            setLastNameWarning(true)
            valid = false
        } else setLastNameWarning(false)
        if (password === "") {
            setPasswordWarning(true)
            valid = false
        } else setPasswordWarning(false)

        if (password !== confirmPassword) {
            setConfirmWarningMessage("Passwords don't match!")
            valid = false
            setConfirmPasswordWarning(true)
            if (confirmPassword === "") {
                setConfirmWarningMessage()
            }
        } else {
            setConfirmWarningMessage()
            setConfirmPasswordWarning(false)

        }

        if (confirmPassword === "") {
            setConfirmPasswordWarning(true)
            setConfirmWarningMessage()
            valid = false
        }



        if (valid) {
            var account = {}
            account.Email = email
            account.Username = userName
            account.FirstName = firstName
            account.LastName = lastName
            account.Password = password

            if (handleRegister(account)) {
                return (<Redirect to="/" />)
            }
        }
    }

    return (<div className="card">
        <p className="registertext">Register:</p>
        <TextInput placeholder="Email" autocomplete="username" handleChange={setEmail} mainStyle={{ width: '75%' }} inputStyle={{ backgroundColor: '#F37F68' }} warning={emailWarning} />
        <TextInput placeholder="Username" handleChange={setUserName} mainStyle={{ width: '75%' }} inputStyle={{ backgroundColor: '#F37F68' }} warning={userNameWarning} warningMessage={userNameWarningMessage} />
        <TextInput placeholder="First Name" handleChange={setFirstName} mainStyle={{ width: '75%' }} inputStyle={{ backgroundColor: '#F37F68' }} warning={firstNameWarning} />
        <TextInput placeholder="Last Name" handleChange={setLastName} mainStyle={{ width: '75%' }} inputStyle={{ backgroundColor: '#F37F68' }} warning={lastNameWarning} />

        <TextInput placeholder="Password" autocomplete="new-password" handleChange={setPassword} mainStyle={{ width: '75%', marginTop: '20px' }} inputStyle={{ backgroundColor: '#F37F68' }} hidden={true} warning={passwordWarning} />
        <TextInput placeholder="Confirm Password" handleChange={setConfirmPassword} mainStyle={{ width: '75%', marginBottom: "30px" }} inputStyle={{ backgroundColor: '#F37F68' }} hidden={true} warning={confirmPasswordWarning} warningMessage={confirmWarningMessage} />
        <div className="buttons">
            <Button text="register" click={register} />
        </div>
    </div>)
}