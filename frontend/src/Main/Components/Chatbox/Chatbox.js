import './Chatbox.css';

import { useState } from 'react';

export default function Chatbox({ onSubmit }) {
    const [label, setLabel] = useState("Send message...");
    const [value, setValue] = useState("");

    const handleChange = (v) => {
        setValue(v)
        console.log(v)
        if (v == "") setLabel("Send message...");
        else setLabel("");
    }

    const handleKeyPress = (e) => {
        if (e.key === 'Enter' && value !== "") {
            onSubmit(value);
        }
    }

    return (
        <div className="Chatbox">
            <input className="ChatInput" type="text" id="chatbox" onChange={(e) => handleChange(e.target.value)} onKeyPress={handleKeyPress}></input>
            <label className="ChatLabel noselect" htmlFor="chatbox">{label}</label>
            
        </div>
    )
}