import './Sidebar.css';
import '../Components/Friend/Friend.css';

import Badge from '@mui/material/Badge';
import Avatar from '@mui/material/Avatar';
// import Friend from '../Components/Friend/Friend';

export default function Sidebar({...props}) {
    if (props.friends === undefined) return (<div></div>)

    return(
        <ul className = "Sidebar">
            {props.friends.map(friend => {
                console.log(props)
                return (<div className={friend.Username === props.active ? "Friend Active noselect" : "Friend noselect"} onClick={() => (props.setNewActive(friend.Username))}>
                <Badge
                    overlap="circular"
                    anchorOrigin={{ vertical: "bottom", horizontal: "right" }}
                    variant="dot"
                >
                    <Avatar alt={friend.Username}>{friend.Username[0].toUpperCase()}</Avatar>
                </Badge>
            {friend.Username}
            </div>)
            })}
        </ul>
    )

}