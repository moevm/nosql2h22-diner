import React from 'react';
import { Shifts } from './Shifts';
import { useLoaderData } from 'react-router-dom';

export const userIdLoader = ({ params }: { params: any }) => {
  return params.id;
};

export const UserShifts: React.FC = () => {
  const id: string = useLoaderData() as string;
  React.useEffect(() => {
    if (!id) return;
  }, [id]);
  return <Shifts id={id!} />;
};
