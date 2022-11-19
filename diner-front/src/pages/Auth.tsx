import React from 'react';
import { Button, Checkbox, Form, Input, message } from 'antd';
import { useLogIn, useWhoAmI } from '../api/dinerComponents';
import { useNavigate } from 'react-router-dom';

export const Auth: React.FC = () => {
  const user = useWhoAmI({});
  const navigate = useNavigate();
  const login = useLogIn();

  React.useEffect(() => {
    if (user.error || user.isLoading) return;
    if (user.data) navigate('/dashboard');
  }, [user.data]);

  React.useEffect(() => {
    if (user.error || login.error) {
      message.error(user.error?.payload || login.error?.payload);
    }
  }, [user.error, login.error]);

  const onFinish = React.useCallback(
    (values: { password: string; username: string }) => {
      login
        .mutateAsync({
          body: {
            login: values.username,
            password: values.password,
          },
        })
        .then(() => {
          navigate('/dashboard');
        });
    },
    [navigate],
  );

  return (
    <main className="auth-page">
      <h1>Dinner.noSql</h1>
      <Form
        name="basic"
        labelCol={{ span: 8 }}
        wrapperCol={{ span: 16 }}
        initialValues={{ remember: true }}
        onFinish={onFinish}
        autoComplete="off"
      >
        <Form.Item
          label="Username"
          name="username"
          rules={[{ required: true, message: 'Please input your username!' }]}
        >
          <Input disabled={user.isLoading} />
        </Form.Item>

        <Form.Item
          label="Password"
          name="password"
          rules={[{ required: true, message: 'Please input your password!' }]}
        >
          <Input.Password disabled={user.isLoading} />
        </Form.Item>

        <Form.Item wrapperCol={{ offset: 8, span: 16 }}>
          <Button type="primary" htmlType="submit" loading={user.isLoading}>
            Submit
          </Button>
        </Form.Item>
      </Form>
    </main>
  );
};
