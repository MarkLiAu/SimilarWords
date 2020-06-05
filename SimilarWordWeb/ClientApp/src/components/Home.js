import React, { useState } from 'react';
import { Link, Redirect } from 'react-router-dom'
import { GetLoginUser } from './CommTools';
import { DashBoard } from './DashBoard';
import WordSearchInput  from './WordSearchInput';

const UserFeatures = () => {

}

export const Home = ({ history }) => {

    const handleWordInout = (word) => {
        console.log('home handlewordinput:' + word);
        history.push('/Wordsearch/' + word);
    }
    console.log("Home start");
    let user = GetLoginUser();

    if (user === null)
        return <WordSearchInput handleSubmit={handleWordInout} ></WordSearchInput>
  
    return (
            <div>
                <h3>Welcome {user.firstName }!   </h3>
                <DashBoard></DashBoard>
                <WordSearchInput handleSubmit={handleWordInout} ></WordSearchInput>
            </div>
        );

}
