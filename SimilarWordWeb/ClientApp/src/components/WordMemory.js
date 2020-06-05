import React, { Component, Fragment, useState , useReducer } from 'react';
import { Link } from 'react-router-dom';
import {Button, Col, Grid, Row,Checkbox, Panel, PanelGroup, Carousel } from 'react-bootstrap';
import WordInfoDisplay from './WordInfoDisplay';
import { GetTokenHeader } from './CommTools';


const WordMemory = () => {
    const initialState = {
        firstFlag: 0, 
        memoryList: [],
        memoryIdx: 0,
        showDetail: false,
        curWord: { easiness: 0, viewInterval: 0 }
    }
    const [state, setState] = useReducer(
        (state, newState) => ({ ...state, ...newState }),
        initialState
    );

    const setMemoryList = (data) => setState({ memoryList: data });
    const setFirstFlag = (data) => setState({ firstFlag: data });
    const setMemoryIdx = (data) => setState({ memoryIdx: data }) ;
    const setShowDetail = (data) => setState({ showDetail: data });
    const setCurWord = (data) => setState({ curWord: data });

    //const [memoryList, setMemoryList] = useState([]);
    //const [, setFirstFlag] = useState(0);
    //const [, setMemoryIdx] = useState(0);
    //const [, setShowDetail] = useState(false);
    //const [, setCurWord] = useState({easiness:0,viewInterval:0});

    const CallApi = () => {
        fetch('api/memory/', {
            headers: {
                'Content-Type': 'application/json',
                'Authorization': GetTokenHeader()
            }
            })
            .then(response => response.json())
            .then(data => {
                console.log('fetch back');
                state.memoryList = data;        // can't wait for setMemoryList
                setMemoryList(data);
                ResetCurWord(0);
                setMemoryIdx(0);
            });
    }
    const CallUpdateApi = (e) => {
        //alert("in CallUpdateApi");
        console.log("CallUpdateApi");
        e.preventDefault();
        state.memoryList[state.memoryIdx].easiness = state.curWord.easiness;
        state.memoryList[state.memoryIdx].viewInterval = state.curWord.viewInterval;
        setMemoryList(state.memoryList);
        fetch('api/memory/' + state.memoryList[state.memoryIdx].name, {
            method: 'PUT',
            body: JSON.stringify(state.memoryList[state.memoryIdx]),
            headers: {
                'Content-Type': 'application/json',
                'Authorization': GetTokenHeader()
            }
        }).then(response => {
            console.log(response);
            state.memoryList[state.memoryIdx].easiness = 999;
            console.log('word memory after fetch back, idx=' + state.memoryIdx);
            if (state.memoryIdx < state.memoryList.length - 1) {
                setMemoryIdx(state.memoryIdx + 1);
                console.log('word memory after setmemoryIdx, idx=' + state.memoryIdx);
                ResetCurWord(state.memoryIdx + 1);
            }
            setShowDetail(false);
        })
    }

    const ResetCurWord = (idx) => {
        console.log("resetcurWord, idx=" + idx + ",len=" + state.memoryList.length);
        if (state.memoryList.length > 0) {
            state.curWord.easiness = state.memoryList[idx].easiness;
            state.curWord.viewInterval = state.memoryList[idx].viewInterval;
            if (state.curWord.easiness < -2 || state.curWord.easiness > 2) state.curWord.easiness = 0;
            if (state.curWord.viewInterval < 0) state.curWord.viewInterval = 0;
        }
        setCurWord(state.curWord);
    }

    const ChangeEasiness = (e, increase) => {
        //let increase = e.srcElement.id == 'btn_hard' ? 1 : -1;
        console.log('in changeeasiness');
        console.log(increase);
        console.log(state.curWord);
        let v = Math.round(state.curWord.viewInterval * 0.1,0);
        if (v < 1) v = 1;
        state.curWord.viewInterval += v*increase;
        if (state.curWord.viewInterval <= 0) state.curWord.viewInterval = 0;
        if (increase > 0 && state.curWord.viewInterval <= 0) state.curWord.viewInterval = 1;

        if (state.curWord.viewInterval > state.memoryList[state.memoryIdx].viewInterval) state.curWord.easiness = 1;
        else if (state.curWord.viewInterval < state.memoryList[state.memoryIdx].viewInterval) state.curWord.easiness = -1;
        else state.curWord.easiness = 0;

        setCurWord(state.curWord);
        setFirstFlag(state.firstFlag + 1);
        console.log(state.curWord);
    }

    const showDetailClicked = () => { setShowDetail(!state.showDetail)}

    const backClicked = () => {
        if (state.memoryIdx > 0) {
            setShowDetail(false);
            setMemoryIdx(state.memoryIdx - 1);
            ResetCurWord(state.memoryIdx - 1);
        }
    }
    const nextClicked = () => {
        if (state.memoryIdx < state.memoryList.length - 1) {
            setShowDetail(false);
            setMemoryIdx(state.memoryIdx + 1);
            ResetCurWord(state.memoryIdx + 1);
         }
    }

    const reviewDays = (x) => {
        console.log('cal reviewdays:' + x + "," + state.curWord.viewInterval);
        let d = state.curWord.viewInterval;
        return (d <= 0 ? 'Review Today' : 'Review in ' + d + ' day' + (d < 1 ? 's' : '')
        );
    } 

    if (state.firstFlag == 0) {
        CallApi();
        setFirstFlag(1);
    }

    if (state.memoryList.length <= 0) return state.firstFlag<=0 ? 'Loading' : 'no items to review!';

    let nwaiting = 0; state.memoryList.map(x =>nwaiting+=(x.easiness < 999? 1:0));
    if (nwaiting === 0) return 'Congraduation, you finished: ' + state.memoryList.length;

    console.log('memoryList.Length=' + state.memoryList.length.toString() + ',idx=' + state.memoryIdx);
    console.log(state.memoryList[state.memoryIdx]);
        return (
                <Panel bsStyle="primary">
                    <Panel.Heading>
                    <Panel.Title componentClass="h3">
                        {state.memoryList[state.memoryIdx].name}{' (' + (state.memoryList[state.memoryIdx].totalViewed <= 0 ? '*' : '') + (state.memoryIdx + 1).toString() + '/' + state.memoryList.length.toString()+')'}
                        {'   '}<button className='btn btn-info' id='btn_submit' disabled={state.memoryList[state.memoryIdx].easiness >= 999} onClick={CallUpdateApi}>  {reviewDays(state.curWord.viewInterval)}  </button>
                    </Panel.Title>
                    </Panel.Heading>
                    <Panel.Body>

                    <button className='btn btn-danger' id='btn_hard' disabled={state.memoryList[state.memoryIdx].easiness >=999} onClick={(e)=>ChangeEasiness(e,-1)}>Hard</button>{' '}
                    <button className='btn btn-success' id='btn_easy' disabled={state.memoryList[state.memoryIdx].easiness >=999} onClick={(e) => ChangeEasiness(e, 1)}>Easy</button>
                    {'      '} 
                    <button className='btn' id='btn_back' onClick={backClicked}>{'<Back'}</button>
                    {' '} 
                    <button className='btn'  id='btn_next' onClick={nextClicked}>{'Next>'}</button>
                    {' '}
                        <button className='btn' id='btn_detail' onClick={showDetailClicked}>{'Detail...'}</button>
                    
                    </Panel.Body>
                <WordInfoDisplay word={state.showDetail ? state.memoryList[state.memoryIdx]:null}></WordInfoDisplay>
                </Panel>
        );
}
        
        
export default WordMemory;

