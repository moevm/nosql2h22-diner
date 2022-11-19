import { createBrowserRouter, RouterProvider, Route } from 'react-router-dom';
import { Auth } from './pages/Auth';
import { Dashboard } from './pages/Dashboard';
import { Shifts } from './pages/Shifts';
import { ErrorPage } from './pages/Error';
import { Payments } from './pages/Payments';
import { Payment } from './pages/Payment';
import { Resources } from './pages/Resources';
import { Resource, resourceIdLoader } from './pages/Resource';
import { ResourceAdd } from './pages/ResourceAdd';
import { useWhoAmI } from './api/dinerComponents';
import { Button, Result, Spin } from 'antd';
import { Home } from './pages/Home';
import { userIdLoader, UserShifts } from './pages/UserShift';
import { UsersWithSearch } from './pages/UsersWithSearch';
import { ResultStatusType } from 'antd/es/result';
import { UserListWithSearchAndTimePicker } from './pages/UsersWithSearchForShifts';
import { User, userCardIdLoader } from './pages/User';
import { DishesWithSearch } from './pages/DishesWithSearch';
import { ResourcesWithSearch } from './pages/ResourcesWithSearch';
import { Dish, dishIdLoader } from './pages/Dish';

const router = createBrowserRouter([
  {
    path: '/',
    element: <Home />,
    errorElement: <ErrorPage />,
  },
  {
    path: '/auth',
    element: <Auth />,
    errorElement: <ErrorPage />,
  },
  {
    path: '/dashboard',
    element: <Dashboard />,
    errorElement: <ErrorPage />,
    children: [
      {
        path: 'shifts',
        element: <UserListWithSearchAndTimePicker />,
      },
      {
        path: 'shifts/:id',
        element: <UserShifts />,
        loader: userIdLoader,
      },
      {
        path: 'menu',
        element: <DishesWithSearch />,
      },
      {
        path: 'menu/:id',
        element: <Dish />,
        loader: dishIdLoader,
      },
      {
        path: 'staff',
        element: <UsersWithSearch />,
      },
      {
        path: 'staff/:id',
        element: <User />,
        loader: userCardIdLoader,
      },
      {
        path: 'resources',
        element: <ResourcesWithSearch />,
      },
      {
        path: 'resources/:id',
        element: <Resource />,
        loader: resourceIdLoader,
      },
      {
        path: 'resources/add',
        element: <ResourceAdd />,
      },
      {
        path: 'payments',
        element: <Payments />,
      },
      {
        path: 'payments/:id',
        element: <Payment />,
      },
    ],
  },
]);

export const App: React.FC = () => {
  const user = useWhoAmI({});

  if (user.isLoading) {
    return (
      <div className="app-root">
        <Spin />
      </div>
    );
  }
  if (user.error) {
    return (
      <div id="error-page">
        <Result
          status={user.error.status as ResultStatusType}
          title="Error"
          subTitle={user.error.status || user.error.payload}
          extra={
            <Button type="primary" onClick={() => user.refetch()}>
              Try again
            </Button>
          }
        />
      </div>
    );
  }

  return (
    <div className="app-root">
      <RouterProvider router={router} />
    </div>
  );
};
