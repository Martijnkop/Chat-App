import Badge from '@mui/material/Badge';
import Avatar from '@mui/material/Avatar';

import './Friend.css';

const Friend = (friend, active, setActive) => {
    return (
        <div className={friend.friend.Username === active ? "Friend Active noselect" : "Friend noselect"}>
            <Badge
                overlap="circular"
                anchorOrigin={{ vertical: "bottom", horizontal: "right" }}
                variant="dot"
            >
                <Avatar alt={friend.friend.Username}>{friend.friend.Username[0].toUpperCase()}</Avatar>
            </Badge>
        {friend.friend.Username}
        </div>
    )
}

export default Friend;