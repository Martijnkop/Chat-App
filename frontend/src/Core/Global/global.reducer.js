import { Actions } from "./global.actions";

const initialState = {
    connection: undefined
}

export default function globalReducer(state = initialState, action) {
    switch (action.type) {
        case Actions.setConnection:
            return { ...state, connection: action.payload.connection }
        default:
            return state;
    }
}