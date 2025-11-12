// login.jsx
import React, { useState } from "react";
import { login } from "../api/authApi";

const Login = () => {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [message, setMessage] = useState("");

  const handleLogin = async () => {
    const result = await login(email, password);
    setMessage(result.success ? "Вхід виконано успішно!" : result.message);
  };

  return (
    <div style={styles.container}>
      <h2>Вхід</h2>
      <input
        type="email"
        placeholder="Email"
        onChange={(e) => setEmail(e.target.value)}
        style={styles.input}
      />
      <input
        type="password"
        placeholder="Пароль"
        onChange={(e) => setPassword(e.target.value)}
        style={styles.input}
      />
      <button onClick={handleLogin} style={styles.button}>
        Увійти
      </button>
      <p>{message}</p>
    </div>
  );
};

const styles = {
  container: { maxWidth: 400, margin: "100px auto", textAlign: "center" },
  input: { display: "block", margin: "10px auto", padding: "10px", width: "100%" },
  button: { padding: "10px 20px", background: "#0078ff", color: "#fff", border: "none" },
};

export default Login;