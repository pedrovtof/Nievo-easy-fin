import React, { useState, useEffect } from 'react';
import TransactionsView from './View';
import { getTransactionsData } from './api';

/**
 * Transactions Page Controller
 * Fetches and manages the state for the user's transaction history.
 *
 * @returns {JSX.Element} The rendered Transactions page controller.
 */
const Transactions = () => {
  const [data, setData] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchData = async () => {
      try {
        setLoading(true);
        const response = await getTransactionsData();
        const responseData = response.data || response;
        if (responseData.data) {
          setData(responseData.data);
        } else {
          setData(responseData);
        }
      } catch (err) {
        console.error(err);
        setError('Failed to load transactions data');
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, []);

  return <TransactionsView data={data} loading={loading} error={error} />;
};

export default Transactions;
