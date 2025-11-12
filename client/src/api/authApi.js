export const login = async (email, password) => {
  if (email === "test@test.com" && password === "12345678") {
    return { success: true, token: "fake-jwt-token" };
  } else {
    return { success: false, message: "Невірний логін або пароль" };
  }
};

export const register = async (email, password) => {
  return { success: true, message: "Користувача зареєстровано успішно!" };
};

export const logout = async () => {
  return { success: true };
};
