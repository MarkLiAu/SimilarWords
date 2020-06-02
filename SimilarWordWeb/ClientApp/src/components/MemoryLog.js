import React, { Component, Fragment, useState } from 'react';
import { Link } from 'react-router-dom';
import { Col, Grid, Row, Checkbox, Panel, PanelGroup, Table } from 'react-bootstrap';
import { GetTokenHeader } from './CommTools';


export const MemoryLog = ()=> {
    const [userInput, setUserInput] = useState('10');
    const [memLog, setMemLog] = useState([]);

    const InputChanged = (e) => {
        setUserInput(e.target.value);
    }

    const SearchLog = () => {
        console.log('memorylog:' + userInput);
        if (userInput.length <= 0) return;
        fetch('api/Memory/' + userInput, {
            headers: {
                'Content-Type': 'application/json',
                'Authorization': GetTokenHeader()
            }
        })
            .then(response => response.json())
            .then(data => {
                console.log('fetch back');
                setMemLog(data);
            });
    }

    const KeyPressed = (event) => {
        if (event.key === 'Enter') {
            SearchLog();
        }
    }

    const ShowLog = ({ log }) => {
        return (
            log.map((d, idx) => <tr key={idx}>
                <td>{new Date(d.viewTime).toLocaleString()}</td>
                <td>{d.name}</td>
                <td>{d.easiness}</td>
                <td>{d.viewInterval}</td>
            </tr>
            )
            )
    }

    console.log('render');
    return (
        <div className='bj_center'>
                Days to load:<input defaultValue={userInput} placeholder='days back' onChange={InputChanged} onKeyPress={KeyPressed} ></input>
                <span>{' '}</span>
                <button type="submit" onClick={SearchLog}>Load</button>
                <Table striped bordered condensed hover>
                    <thead>
                        <tr>
                            <th>Time</th>
                            <th>Word</th>
                            <th>easiness</th>
                            <th>Interval</th>
                        </tr>
                    </thead>
                    <tbody>
                    <ShowLog log={memLog}></ShowLog>
                    </tbody>
            </Table>

        </div>
    );
}

