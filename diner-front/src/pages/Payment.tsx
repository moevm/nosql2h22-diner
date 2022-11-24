import React from 'react';
import { Card } from 'antd';
import { Link, useParams } from 'react-router-dom';
import { ArrowLeftOutlined } from '@ant-design/icons';
import { useGetPayment } from '../api/dinerComponents';

export const Payment: React.FC = () => {
	const { id } = useParams();
	const paymentData = useGetPayment({ queryParams: { id } });

	return (
		<div>
			<Link to="/dashboard/payments">
				<ArrowLeftOutlined /> Go back to payments
			</Link>
			<br />
			<br />
			<Card title={`Payment #${paymentData.data?.number}`} style={{ width: 501 }}>
				<Card.Grid style={{ width: 250 }}>Description</Card.Grid>
				<Card.Grid style={{ width: 250 }}>{paymentData.data?.description}</Card.Grid>
				<Card.Grid style={{ width: 250 }}>Price</Card.Grid>
				<Card.Grid style={{ width: 250 }}>$ {paymentData.data?.price}</Card.Grid>
				<Card.Grid style={{ width: 250 }}>Status</Card.Grid>
				<Card.Grid style={{ width: 250 }}>{paymentData.data?.status}</Card.Grid>
			</Card>
		</div>
	);
};
