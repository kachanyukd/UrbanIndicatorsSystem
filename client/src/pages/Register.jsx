// register.jsx
import React, { useState } from "react";
import { register } from "../api/authApi";

const Register = () => {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [message, setMessage] = useState("");

  const handleRegister = async () => {
    const result = await register(email, password);
    setMessage(result.message);
  };

  return (
    <div style={styles.container}>
      <h2>Реєстрація</h2>
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
      <button onClick={handleRegister} style={styles.button}>
        Зареєструватися
      </button>
      <p>{message}</p>
    </div>
  );
};

const styles = {
  container: { maxWidth: 400, margin: "100px auto", textAlign: "center" },
  input: { display: "block", margin: "10px auto", padding: "10px", width: "100%" },
  button: { padding: "10px 20px", background: "#28a745", color: "#fff", border: "none" },
};

export default Register;