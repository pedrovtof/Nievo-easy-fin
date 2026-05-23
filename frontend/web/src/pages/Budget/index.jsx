import React, { useState, useEffect } from 'react';
import BudgetView from './View';
import { getBudgetData } from './api';

const Budget = () => {
  const [data, setData] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchData = async () => {
      try {
        setLoading(true);
        const response = await getBudgetData();
        const responseData = response.data || response;
        if (responseData.data) {
          setData(responseData.data);
        } else {
          setData(responseData);
        }
      } catch (err) {
        console.error(err);
        setError('Failed to load budget data');
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, []);

  return <BudgetView data={data} loading={loading} error={error} />;
};

export default Budget;
