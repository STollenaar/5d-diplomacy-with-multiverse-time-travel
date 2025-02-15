import React, { useState } from 'react';
import './LoginPage.css'; // Import the CSS file for styling
import useLogin from '../../hooks/api/useLogin';

const LoginPage = ({ onLogin }: { onLogin: (token: string) => void }) => {
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [error, setError] = useState('');
    const { login, user } = useLogin();

    const handleLogin = async (e: React.FormEvent) => {
        e.preventDefault();
        setError('');

        login(
            { username, password },
            {
                onSuccess: (data) => {
                    const { id } = data;
                    localStorage.setItem('userId', id.toString());
                    onLogin(id.toString());
                },
                onError: () => {
                    setError('Invalid username or password');
                },
            }
        );
    };

    return (
        <div className="login-container">
            <h2>Login</h2>
            {error && <p className="error">{error}</p>}
            <form onSubmit={handleLogin}>
                <div className="form-group">
                    <label htmlFor="username">Username:</label>
                    <input
                        type="text"
                        id="username"
                        value={username}
                        onChange={(e) => setUsername(e.target.value)}
                    />
                </div>
                <div className="form-group">
                    <label htmlFor="password">Password:</label>
                    <input
                        type="password"
                        id="password"
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                    />
                </div>
                <button type="submit">Login</button>
            </form>
        </div>
    );
};

export default LoginPage;