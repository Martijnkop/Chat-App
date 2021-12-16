import React from "react";
import './TextInput.css';

export default function TextInput({ handleChange, placeholder, mainStyle, inputStyle, labelStyle, hidden, warning, value, warningMessage, autocomplete }) {
    return (
        <div className={warning ? "textInput warning" : "textInput"} style={mainStyle}>
            <input autoComplete={autocomplete} className="input" style={inputStyle} type={hidden ? "password" : "text"} placeholder=" " id={"input_" + placeholder} onChange={(e) => handleChange(e.target.value)} value={value}></input>
            <label style={labelStyle} htmlFor={"input_" + placeholder}>{warning ? warningMessage !== undefined ? warningMessage : placeholder + " required!" : placeholder}</label>
        </div>
    )
}