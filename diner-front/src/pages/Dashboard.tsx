import React from 'react';
import { PageHeader } from 'antd';
import { Avatar } from 'antd';
import { CalendarOutlined, UserOutlined, CoffeeOutlined, TeamOutlined, DollarCircleOutlined, GoldOutlined } from '@ant-design/icons';
import { Menu } from 'antd';
import { Outlet, useNavigate } from "react-router-dom";


export const Dashboard: React.FC = () => {
  const navigate = useNavigate();
  const menuItems = React.useMemo(() => ([
    { label: 'Shifts', key: 'shifts', icon: <CalendarOutlined />, onClick: () => navigate('/dashboard/shifts') },
    { label: 'Menu', key: 'menu', icon: <CoffeeOutlined />, onClick: () => navigate('/dashboard/menu') },
    { label: 'Staff', key: 'staff', icon: <TeamOutlined />, onClick: () => navigate('/dashboard/staff') },
    { label: 'Payments', key: 'payments', icon: <DollarCircleOutlined />, onClick: () => navigate('/dashboard/payments') },
    { label: 'Resources', key: 'resources', icon: <GoldOutlined />, onClick: () => navigate('/dashboard/resources') },
  ]), [])

  return <div>
    <header>
      <PageHeader
        className="page-header"
        title="Diner.noSql"
        extra={[<div key="avatar"><Avatar icon={<UserOutlined />} /> John Doe</div>]}
      />
    </header>
    <div className='page-wrapper'>
      <Menu items={menuItems} className='navigation' />
      <main className='page-content'>
        <Outlet />
      </main>
    </div>
  </div>
}