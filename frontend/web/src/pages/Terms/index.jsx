import React, { useState, useEffect } from 'react';
import TermsView from './View';
import { getAcceptTerms } from './api';

/**
 * Terms Page Controller
 * Fetches the current terms of use content from the API and passes it to the View.
 * This page is opened in a new tab via window.open('/terms', '_blank').
 */
const Terms = () => {
  const [content, setContent] = useState('');
  const [version, setVersion] = useState(null);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    const fetchTerms = async () => {
      try {
        const res = await getAcceptTerms();
        // Real API: { success, data: { content, version } }
        // Mock:     { success, content, version }
        const data = res?.data?.data ?? res?.data ?? {};
        setContent(data.content ?? '');
        setVersion(data.version ?? null);
      } catch {
        setError('Não foi possível carregar os termos de uso. Tente novamente mais tarde.');
      } finally {
        setIsLoading(false);
      }
    };

    fetchTerms();
  }, []);

  return <TermsView content={content} version={version} isLoading={isLoading} error={error} />;
};

export default Terms;
