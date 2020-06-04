import React, { useState } from 'react';
import { Link, Redirect } from 'react-router-dom'
import { loginUser } from './CommTools';
import { DashBoard } from './DashBoard';
import WordSearchInput  from './WordSearchInput';

export const Home = ({ history }) => {

    const handleWordInout = (word) => {
        console.log('home handlewordinput:' + word);
        history.push('/Wordsearch/' + word);
    }
        return (
            <div>
                <h3>Welcome {loginUser === null ? 'Guest' : loginUser.firstName}!          </h3>
                <Link to={'/Wordmemory'} ><button className='btn btn-info btn-sm'> Start Memory </button></Link>
                <DashBoard></DashBoard>
                <WordSearchInput handleSubmit={handleWordInout} ></WordSearchInput>
            </div>
        );

}
