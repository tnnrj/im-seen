export default {
  isValidEmail
}

export function isValidEmail(email: string): boolean {
  const regex = /^[\w-]+(\.[\w-]+)*@([a-z0-9-]+(\.[a-z0-9-]+)*?\.[a-z]{2,6}|(\d{1,3}\.){3}\d{1,3})(:\d{4})?$/i;
  return !!email && !!regex.exec(email);
}