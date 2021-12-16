export default function Message(props) {

    console.log(props)

    return (<div className= {"Message " + (props.me === props.username ? "Me" : "")} >
        <div className="name">{props.username}</div>
        <div className="content">{props.content}</div>
    </div>)
}