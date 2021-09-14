import GetLocalStorage from "./GetLocalStorage";

function SetLocalStorage(darkmode) {
    localStorage.setItem('darkmode', darkmode === undefined ? true : darkmode);
    localStorage.setItem('account', {})

    return GetLocalStorage();
}

export default SetLocalStorage;