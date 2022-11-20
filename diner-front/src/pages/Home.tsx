import React, { useDeferredValue } from 'react';
import { Button, Checkbox, Form, Input } from 'antd';
import { useLogIn, useWhoAmI } from '../api/dinerComponents';
import { useNavigate } from 'react-router-dom';

export const Home: React.FC = () => {
	const user = useWhoAmI({});
	const navigate = useNavigate();

	React.useEffect(() => {
		if (user.error || user.isLoading) return;
		console.log(`here is ${user.data}`);
		if (!user.data) {
			navigate('/auth');
		} else {
			console.log('kek');
			navigate('/dashboard');
		}
		console.log(user.data);
	}, [user.data]);

	return <div>TBD home</div>;
};
