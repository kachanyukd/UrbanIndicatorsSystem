// navbar.jsx
import React from "react";

const Navbar = () => {
  return (
    <nav style={styles.nav}>
      <h2 style={styles.logo}>Urban Indicators</h2>
      <ul style={styles.menu}>
        <li>Головна</li>
        <li>Трафік</li>
        <li>Райони</li>
        <li>Профіль</li>
      </ul>
    </nav>
  );
};

const styles = {
  nav: {
    backgroundColor: "#1f1f1f",
    color: "white",
    padding: "10px 30px",
    display: "flex",
    justifyContent: "space-between",
    alignItems: "center",
  },
  logo: {
    margin: 0,
  },
  menu: {
    listStyle: "none",
    display: "flex",
    gap: "20px",
  },
};

export default Navbar;