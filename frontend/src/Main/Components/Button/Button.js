import './Button.css'

export default function Button({ text, click }) {
    return (<div className="button" onClick={(e) => click()}>{text}</div>);
}