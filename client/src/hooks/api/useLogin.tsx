import axios from 'axios';
import { useMutation } from '@tanstack/react-query';
import User from '../../types/user';
import routes from '../../api/routes';

type LoginRequest = {
    username: string;
    password: string;
};

const login = async ({ username, password }: LoginRequest): Promise<User> => {
    const route = routes.login();
    const response = await axios.post<User>(route, {
        username,
        password,
    });
    return response.data
}

const useLogin = () => {
    const { mutate, data, ...rest } = useMutation({
        mutationKey: ['login'],
        mutationFn: login,
    });
    return { login: mutate, user: data ?? null, rest };
};

export default useLogin;