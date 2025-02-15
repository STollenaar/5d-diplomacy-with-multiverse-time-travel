import React, { useState } from 'react';
import { QueryClientProvider } from '@tanstack/react-query';
import queryClient from './api/queryClient';
import { GameContextProvider } from './components/context/GameContext';
import AppRoot from './components/AppRoot';
import LoginPage from './components/pages/LoginPage';

const App = () => {
    const [isLoggedIn, setIsLoggedIn] = useState(false);
    const [token, setToken] = useState<string | null>(null);

    const handleLogin = (token: string) => {
        setToken(token);
        setIsLoggedIn(true);
    };

    return (
        <QueryClientProvider client={queryClient}>
            <GameContextProvider>
                {isLoggedIn ? <AppRoot token={token} /> : <LoginPage onLogin={handleLogin} />}
            </GameContextProvider>
        </QueryClientProvider>
    );
};

export default App;