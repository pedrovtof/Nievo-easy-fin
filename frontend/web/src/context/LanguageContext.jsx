import React, { createContext, useContext, useState } from 'react';
import { locales } from '../utils/locales';

const LanguageContext = createContext();

export const LanguageProvider = ({ children }) => {
    const [language, setLanguage] = useState('en-us');

    const t = (path) => {
        const keys = path.split('.');
        let current = locales[language];

        for (const key of keys) {
            if (current && current[key]) {
                current = current[key];
            } else {
                console.warn(`Translation missing for key: ${path} in language: ${language}`);
                return path;
            }
        }
        return current;
    };

    return (
        <LanguageContext.Provider value={{ language, setLanguage, t }}>
            {children}
        </LanguageContext.Provider>
    );
};

export const useLanguage = () => useContext(LanguageContext);
