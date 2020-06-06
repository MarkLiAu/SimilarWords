import React, { Fragment } from 'react';
import { Link  } from 'react-router-dom';
import { Badge } from 'react-bootstrap';
import { ShowDictLink } from './CommTools';

const ShowWordName = ({ name, handleClick }) => {
    if (!name || name.trim() === "") return '';
    return (
        handleClick === null ?
            <Link to={{ pathname: '/Wordsearch/' + name }} >   {name}   </Link>
            :
            <a className='a_pointer' onClick={() => { console.log('WordInfoDisplay clicked'); handleClick(name); return false; }} > {name} </a>
    )
}

const ShowSimilarWords = ({ nameString, handleClick }) => {
    //console.log("SHowSimilarWords:" + nameString);
    if (nameString.trim() === "") return '';
    let nameList = nameString.split(' ');
    //console.log(nameList);
    return (
        nameList.map(name => {
            return <ShowWordName key={name} name={name} handleClick={handleClick}></ShowWordName>
        })
    )

}

const DisplaySoundLink = ({ name }) => {
    //return <span> {`https://lex-audio.useremarkable.com/mp3/${name}_us_1.mp3`} </span>
    return <audio src={`https://lex-audio.useremarkable.com/mp3/${name}_us_1.mp3`} controls />
 
}


const WordInfoDisplay = ({ word,hideEditButton, handleWordClicked=null}) => {
    //console.log('WordInfoDisplay:');
    //console.log(word);
    if (!word) return '';

    const playMusic = () => {
        let name = word.name;
        let link1 = `https://lex-audio.useremarkable.com/mp3/${name}_us_1.mp3`;
        let link2 = `https://ssl.gstatic.com/dictionary/static/sounds/oxford/${name}--_us_1.mp3`;
        let mySound = new Audio(link2);
        console.log(mySound.readyState);

        mySound.play().catch(e=>console.log('failed to play sound'));
    }

    return (
        <table className='table table-striped table-bordered'>
            <tbody>
            <tr>
                    <td>Name:</td>
                    <td>
                        <ShowWordName name={word.name} handleClick={handleWordClicked}> </ShowWordName>
                        {' '}
                        <Badge>{word.frequency}</Badge>
                        {' '}
                        <button onClick={playMusic}><span className="glyphicon glyphicon-play  text-success"></span> </button>
                        {' '}
                        <Link to={{ pathname: '/wordedit/' + word.name, state: { word: word } }} >
                            <button hidden={hideEditButton} type="button" > Edit </button>
                        </Link>
                    </td>
            </tr>
            <tr>
                <td>pronounciation:</td><td>{word.pronounciation}</td>
            </tr>
                <tr>
                    <td>Similar words:</td>
                    <td>
                        <ShowSimilarWords nameString={word.similarWords} handleClick={handleWordClicked}>  </ShowSimilarWords>
                    </td>
                </tr>
            <tr>
                <td>meaningShort:</td><td>{word.meaningShort}</td>
            </tr>
                <WordReviewInfo word={word}></WordReviewInfo>
                <tr>
                    <td>Onine dictionary</td><td><ShowDictLink name={word.name}></ShowDictLink></td>
                </tr>

                

            </tbody>
        </table>
    );
}

const WordReviewInfo = ({ word }) => {
    let infoList = [];
    infoList.push({ title: 'Total viewed', value: word.totalViewed + (word.totalViewed<=0 && word.viewInterval>=-1? ' (in memory list)':'') });
    //if (word.totalViewed > 0)
    {
        infoList.push({ title: 'Start Time', value: new Date(word.startTime).toLocaleString() });
        infoList.push({ title: 'View Time', value: new Date(word.viewTime).toLocaleString()  });
        infoList.push({ title: 'Interval', value:word.viewInterval });
        infoList.push({ title: 'Last Easiness', value:word.easiness });
    }
    return (
        infoList.map((x,idx) =>
            <tr key={idx}>
                <td>{x.title + ':'}</td><td>{x.value}</td>
            </tr>
        )
    
    );
}


export default WordInfoDisplay;
