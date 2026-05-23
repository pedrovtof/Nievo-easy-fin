import React, { useState, useEffect } from 'react';
import DashboardView from './View';
import { getDashboardData } from './api';

/**
 * Dashboard Page Controller
 * Fetches and manages the primary financial overview data for the user.
 *
 * @returns {JSX.Element} The rendered Dashboard page controller.
 */
const Dashboard = () => {
  const [data, setData] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchData = async () => {
      try {
        setLoading(true);
        const response = await getDashboardData();
        const responseData = response.data || response;
        if (responseData.data) {
          setData(responseData.data);
        } else {
          setData(responseData);
        }
      } catch (err) {
        console.error(err);
        setError('Failed to load dashboard data');
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, []);

  return <DashboardView data={data} loading={loading} error={error} />;
};

export default Dashboard;
