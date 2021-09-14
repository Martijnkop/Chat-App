function GetLocalStorage() {
    var storage = {};

    storage.darkmode = localStorage.getItem('darkmode');
    storage.account = localStorage.getItem('account');
    return storage
}

export default GetLocalStorage;