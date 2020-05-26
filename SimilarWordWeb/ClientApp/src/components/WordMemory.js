import React, { Component, Fragment, useState } from 'react';
import { Link } from 'react-router-dom';
import { Col, Grid, Row,Checkbox, Panel, PanelGroup, Carousel } from 'react-bootstrap';
import WordInfoDisplay from './WordInfoDisplay';


const WordMemory = () => {

    const [memoryList, setMemoryList] = useState([]);
    const [firstFlag, setFirstFlag] = useState(0);
    const [memoryIdx, setMemoryIdx] = useState(0);
    const [showDetail, setShowDetail] = useState(false);

    const CallApi = () => {
        fetch('api/memory/')
            .then(response => response.json())
            .then(data => {
                console.log('fetch back');
                setMemoryList(data);
            });
    }
    const CallUpdateApi = (e) => {
        //alert("in CallUpdateApi");
        console.log("CallUpdateApi");
        e.preventDefault();
        fetch('api/memory/' +memoryList[memoryIdx].name, {
            method: 'PUT',
            body: JSON.stringify(memoryList[memoryIdx]),
            headers: {
                'Content-Type': 'application/json'
            }
        })
            .then(response => { console.log(response); setShowDetail(false); setMemoryIdx(memoryIdx + 1) })
    }

    const ChangeEasiness = (e, increase) => {
        //let increase = e.srcElement.id == 'btn_hard' ? 1 : -1;
        console.log('in changeeasiness');
        console.log(memoryList[memoryIdx].viewInterval);
        console.log(increase);
        if (memoryList[memoryIdx].easiness > -1 && memoryList[memoryIdx].easiness<1)
            memoryList[memoryIdx].easiness += increase;
        let v = Math.round(memoryList[memoryIdx].viewInterval * 0.1,0);
        if (v < 1) v = 1;
        memoryList[memoryIdx].viewInterval += v*increase;
        if (memoryList[memoryIdx].viewInterval <= 0) memoryList[memoryIdx].viewInterval = 0;

        setMemoryList(memoryList);
        setFirstFlag(firstFlag + 1);
        console.log(memoryList[memoryIdx].viewInterval);
    }

    const showDetailClicked = () => { setShowDetail(!showDetail)}

    const backClicked = () => {
        if (memoryIdx > 0) { setShowDetail(false); setMemoryIdx(memoryIdx - 1) }
    }
    const nextClicked = () => {
        if (memoryIdx < memoryList.length - 1) { setShowDetail(false); setMemoryIdx(memoryIdx + 1) }
    }

    const reviewDays = d => d <= 0 ? 'Review Today' : 'Review in ' + d + ' day' + (d < 1 ? 's' : ''); 

    if (firstFlag == 0) {
        CallApi();
        setFirstFlag(1);
    }

    if (memoryList.length <= 0) return 'no items to review!';

    if (memoryIdx >= memoryList.length) return 'Congraduation, you finished: ' + memoryList.length;

    console.log('memoryList.Length=' + memoryList.length.toString()+',idx='+memoryIdx);
    console.log(memoryList[memoryIdx]);
        return (
                <Panel bsStyle="primary">
                    <Panel.Heading>
                    <Panel.Title componentClass="h3">
                        {memoryList[memoryIdx].name}{' (' + (memoryIdx+1).toString() + '/' + memoryList.length.toString()+')'}
                    </Panel.Title>
                    </Panel.Heading>
                    <Panel.Body>

                    <button id='btn_hard' onClick={(e)=>ChangeEasiness(e,-1)}>Hard</button>{' '}
                    <button onClick={CallUpdateApi}>{reviewDays(memoryList[memoryIdx].viewInterval)}</button>{' '}
                    <button id='btn_easy' onClick={(e) => ChangeEasiness(e, 1)}>Easy</button>
                    {' '} 
                    <button id='btn_back' onClick={backClicked}>{'<'}</button>
                    {' '} 
                    <button id='btn_next' onClick={nextClicked}>{'>'}</button>
                    {' '}
                    <button id='btn_detail' onClick={showDetailClicked}>{'...'}</button>
                    </Panel.Body>
                    <WordInfoDisplay word={showDetail?memoryList[memoryIdx]:null}></WordInfoDisplay>
                </Panel>
        );
}
        
        
export default WordMemory;

