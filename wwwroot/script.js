function generateUserId(length) {
  return [...window.crypto.getRandomValues(new Uint8Array(length))]
    .map(x => String.fromCharCode(x % 26 + 'a'.charCodeAt(0))).join('');
}
