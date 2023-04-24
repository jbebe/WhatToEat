async function getAdOjectAsync() {
    return await(await fetch('/.auth/me', { method: 'GET', 'credentials': 'same-origin' })).json();
}
