export const Actions = {
    setConnection: "[Global] Connection"
}

export const setConnection = (connection) => ({
    type: Actions.setConnection,
    payload: {
        connection
    }
})