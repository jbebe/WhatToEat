async function getAdObjectAsync() {
    return await (await fetch('/.auth/me', { method: 'GET', 'credentials': 'same-origin' })).json();
}
