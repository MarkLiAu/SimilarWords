import React, { useState } from 'react';
import { Link, Redirect } from 'react-router-dom'
import { GetLoginUser } from './CommTools';
import { DashBoard } from './DashBoard';
import WordSearchInput  from './WordSearchInput';

export const Home = ({ history }) => {

    const handleWordInout = (word) => {
        console.log('home handlewordinput:' + word);
        history.push('/Wordsearch/' + word);
    }
    console.log("Home start");
    let user = GetLoginUser();
        return (
            <div>
                <h3>Welcome {user === null ? 'Guest' : user.firstName }!          </h3>
                <Link to={'/Wordmemory'} ><button className='btn btn-info btn-sm'> Start Memory </button></Link>
                <DashBoard></DashBoard>
                <WordSearchInput handleSubmit={handleWordInout} ></WordSearchInput>
            </div>
        );

}
